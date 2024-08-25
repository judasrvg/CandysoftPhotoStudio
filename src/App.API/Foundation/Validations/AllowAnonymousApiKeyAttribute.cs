namespace App.API.Foundation.Validations
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AllowAnonymousApiKeyAttribute : Attribute
    {
    }
}
