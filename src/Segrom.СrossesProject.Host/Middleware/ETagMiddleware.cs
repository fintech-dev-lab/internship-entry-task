using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;

namespace Segrom.СrossesProject.Host.Middleware;

internal class ETagMiddleware(RequestDelegate next, IMemoryCache cache)
{
	
	public async Task InvokeAsync(HttpContext context)
	{
		var etag = context.Request.Headers[HeaderNames.IfNoneMatch].FirstOrDefault();
		if (string.IsNullOrEmpty(etag))
		{
			await next.Invoke(context);
			return;
		}
		
		if (cache.TryGetValue(etag, out _))
		{
			context.Response.Headers[HeaderNames.ETag] = etag;
			return;
		}
		
		cache.Set(etag, etag);
		await next.Invoke(context);
		cache.Remove(etag);
	}
}