namespace Plainly.Api.Interfaces;

public interface IModelHasData<TModel>
{
    public IEnumerable<TModel> GetModelData();
}