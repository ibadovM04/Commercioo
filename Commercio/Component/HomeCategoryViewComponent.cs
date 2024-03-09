using Commercio.Data;
using Commercio.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Commercio.Component
{
	public class HomeCategoryViewComponent:ViewComponent
	{
		private readonly ApplicationDbContext _context;
		private readonly IConfiguration _configuration;
		public HomeCategoryViewComponent(ApplicationDbContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var categories = _context.Categories.Where(c => c.IsMainPage).Select(c => new CategoryDto
			{
				Slogan = c.Slogan,
				CategotyId = c.Id,
				Name = c.Name,
				BackroundImageUrl = _configuration["Categories:Sliders"] + c.BackgroundImageUrl,
				Priority = c.Priority ?? 0
			})
				.OrderBy(c => c.Priority).ToList();   


				
			return View(categories);
		}
	}
}
