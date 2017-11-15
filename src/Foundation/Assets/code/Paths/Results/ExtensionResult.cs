namespace Lotus.Foundation.Assets.Paths.Results
{
    public class ExtensionResult
    {
        public event ProcessResultDelegate OnProcessResult;
        public delegate void ProcessResultDelegate(object[] variables);

        #region Constructor
        public ExtensionResult()
        {
            
        }
        
        public ExtensionResult(ProcessResultDelegate invoke)
        {
            OnProcessResult = invoke;
        }
        #endregion
        
        public virtual void ProcessResult(params object[] variables)
        {
            if (OnProcessResult != null)
            {
                OnProcessResult(variables);
            }
        }
    }
}