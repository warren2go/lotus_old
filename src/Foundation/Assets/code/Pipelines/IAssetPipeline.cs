namespace Lotus.Foundation.Assets.Pipelines
{
    public interface IAssetPipeline
    {
        void Process(AssetPipelineArgs args);
    }
}