namespace Lotus.Foundation.Assets.Pipelines.Request
{
    public class HeadersPipeline : IAssetPipeline
    {
        public void Process(AssetPipelineArgs args)
        {
            ProcessHeaders(args);
        }

        private void ProcessHeaders(AssetPipelineArgs args)
        {
            var headers = Global.Repository.Headers;
            foreach (var header in headers)
            {
                args.Context.Response.Headers[header.Key] = header.Value.Replace("$(timestamp)", args.Timestamp.ToString("0000000000"));
            }
        }
    }
}