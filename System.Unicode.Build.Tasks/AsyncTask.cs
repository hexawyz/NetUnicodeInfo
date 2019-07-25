using System.Collections.Concurrent;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
#if NETSTANDARD2_0
using BuildTask = Microsoft.Build.Utilities.Task;
#else
using BuildTask = Microsoft.Build.Utilities.AppDomainIsolatedTask;
#endif

namespace System.Unicode.Build.Tasks
{
	[RunInMTA]
	[LoadInSeparateAppDomain]
	public abstract class AsyncTask : BuildTask, ICancelableTask
	{
		private sealed class AsyncTaskSynchronizationContext : SynchronizationContext, IDisposable
		{
			private readonly BlockingCollection<(SendOrPostCallback d, object state)> _queuedMessages;
			private readonly SynchronizationContext _oldSynchronizationContext;

			public AsyncTaskSynchronizationContext(BlockingCollection<(SendOrPostCallback d, object state)> queuedMessages)
			{
				_queuedMessages = queuedMessages;
				_oldSynchronizationContext = Current;
				SetSynchronizationContext(this);
			}

			public void Dispose() => SetSynchronizationContext(_oldSynchronizationContext);

			public override void OperationStarted() => throw new NotSupportedException();

			public override void OperationCompleted() => throw new NotSupportedException();

			public override void Post(SendOrPostCallback d, object state) => _queuedMessages.Add((d, state));

			public override void Send(SendOrPostCallback d, object state) => throw new NotSupportedException();
		}

		private CancellationTokenSource _cancellationTokenSource;

		protected AsyncTask()
		{
		}

		protected AsyncTask(ResourceManager taskResources) : base(taskResources)
		{
		}

		protected AsyncTask(ResourceManager taskResources, string helpKeywordPrefix) : base(taskResources, helpKeywordPrefix)
		{
		}

		private static CancellationToken CancelOnCompletion(Task task)
		{
			if (task.IsCompleted) return new CancellationToken(true);

			var cts = new CancellationTokenSource();

			task.ContinueWith
			(
				(t, state) =>
				{
					((CancellationTokenSource)state).Cancel();
				},
				cts,
				TaskContinuationOptions.ExecuteSynchronously
			);

			return cts.Token;
		}

		public sealed override bool Execute()
		{
			_cancellationTokenSource = new CancellationTokenSource();
			try
			{
				var queuedMessages = new BlockingCollection<(SendOrPostCallback callback, object state)>();

				using (new AsyncTaskSynchronizationContext(queuedMessages))
				{
					var task = ExecuteAsync(_cancellationTokenSource.Token);

					var ct = CancelOnCompletion(task);

					while (!ct.IsCancellationRequested)
					{
						SendOrPostCallback callback;
						object state;

						try
						{
							(callback, state) = queuedMessages.Take(ct);
						}
						catch (OperationCanceledException) when (ct.IsCancellationRequested || _cancellationTokenSource.IsCancellationRequested)
						{
							break;
						}

						callback(state);
					}

					return task.Result;
				}
			}
			finally
			{
				_cancellationTokenSource.Dispose();
				_cancellationTokenSource = null;
			}
		}

		public void Cancel() => _cancellationTokenSource?.Cancel();

		protected abstract Task<bool> ExecuteAsync(CancellationToken cancellationToken);
	}
}
