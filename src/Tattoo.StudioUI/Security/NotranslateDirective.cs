namespace Tattoo.StudioUI.Security
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Rendering;

    public class NotranslateDirective : ComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(1, "div");
            builder.AddAttribute(2, "class", "notranslate");
            builder.AddContent(3, ChildContent);
            builder.CloseElement();
        }
    }
}
