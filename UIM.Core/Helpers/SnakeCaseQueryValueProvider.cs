using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UIM.Core.Helpers;

public class SnakeCaseQueryValueProvider
    : QueryStringValueProvider,
      Microsoft.AspNetCore.Mvc.ModelBinding.IValueProvider
{
    public SnakeCaseQueryValueProvider(
        BindingSource bindingSource,
        IQueryCollection values,
        CultureInfo culture
    ) : base(bindingSource, values, culture) { }

    public override bool ContainsPrefix(string prefix)
    {
        return base.ContainsPrefix(prefix.ToSnakeCase());
    }

    public override ValueProviderResult GetValue(string key)
    {
        return base.GetValue(key.ToSnakeCase());
    }
}

public class SnakeCaseQueryValueProviderFactory : IValueProviderFactory
{
    public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var valueProvider = new SnakeCaseQueryValueProvider(
            BindingSource.Query,
            context.ActionContext.HttpContext.Request.Query,
            CultureInfo.CurrentCulture
        );

        context.ValueProviders.Add(valueProvider);

        return Task.CompletedTask;
    }
}
