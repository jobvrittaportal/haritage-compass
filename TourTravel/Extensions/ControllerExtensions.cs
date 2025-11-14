using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading.Tasks;

public static class ControllerExtensions
{
  public static async Task<string> RenderViewComponentToStringAsync(
      this Controller controller,
      string componentName,
      object parameters = null)
  {
    var services = controller.HttpContext.RequestServices;

    var viewComponentHelper = services.GetRequiredService<IViewComponentHelper>();

    // Fake view context (required for helper)
    var viewContext = new ViewContext(
        controller.ControllerContext,
        new FakeView(),
        controller.ViewData,
        controller.TempData,
        TextWriter.Null,
        new HtmlHelperOptions()
    );

    // Bind ViewContext
    if (viewComponentHelper is IViewContextAware contextAware)
    {
      contextAware.Contextualize(viewContext);
    }

    // Execute ViewComponent
    using var writer = new StringWriter();
    var result = await viewComponentHelper.InvokeAsync(componentName, parameters);
    result.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);

    return writer.ToString();
  }

  private class FakeView : IView
  {
    public string Path => string.Empty;

    public Task RenderAsync(ViewContext context)
    {
      return Task.CompletedTask;
    }
  }
}
