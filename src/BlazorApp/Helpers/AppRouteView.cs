using BlazorApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Net;

namespace BlazorApp.Helpers
{
    public class AppRouteView : RouteView
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IAccountService AccountService { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
            var authorize = Attribute.GetCustomAttribute(RouteData.PageType, typeof(AuthorizeAttribute)) != null;
            AuthorizeAttribute authorizeAttribute = (AuthorizeAttribute)Attribute.GetCustomAttribute(RouteData.PageType, typeof(AuthorizeAttribute));
            if (authorize && AccountService.User == null)
            {
                var returnUrl = WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery);
                NavigationManager.NavigateTo($"account/login?returnUrl={returnUrl}");
            }
            else if (
                authorizeAttribute != null &&
                AccountService.User != null &&
                authorizeAttribute.Roles != null &&
                authorizeAttribute.Roles != AccountService.User.Role
            )
            {
                // when the user is not permitted because of its role.
                NavigationManager.NavigateTo($"/");
            }
            else
            {
                base.Render(builder);
            }
        }
    }
}