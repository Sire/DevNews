﻿using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NewsletterCurator.Data;
using NewsletterCurator.HTMLScraper;
using NewsletterCurator.Web.Models;
using Newtonsoft.Json;

namespace NewsletterCurator.Web.Controllers
{
    public class EmailController : Controller
    {
        private readonly NewsletterCuratorContext newsletterCuratorContext;
        private readonly HTMLScraperService htmlScraperService;
        private readonly EmailService.EmailService emailService;
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;

        public EmailController(NewsletterCuratorContext newsletterCuratorContext, HTMLScraperService htmlScraperService, EmailService.EmailService emailService, IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            this.newsletterCuratorContext = newsletterCuratorContext;
            this.htmlScraperService = htmlScraperService;
            this.emailService = emailService;
            this.configuration = configuration;
            this.clientFactory = clientFactory;
        }

        public async Task<IActionResult> Send()
        {
            var src = await htmlScraperService.ScrapeAsync(Url.AbsoluteAction("Preview", "Email"));

            var newsletterFilename = $"{DateTimeOffset.UtcNow.ToString("yyyy-MM-dd")}.html";

            var result = await addToGitHubArchive(src, newsletterFilename);

            await emailService.SendAsync(src, await newsletterCuratorContext.Subscribers.Where(s => s.DateUnsubscribed == null && s.DateValidated != null).Select(s => s.Email).ToListAsync());

            newsletterCuratorContext.Newsitems.RemoveRange(newsletterCuratorContext.Newsitems);
            await newsletterCuratorContext.SaveChangesAsync();

            return RedirectToAction("Share", new { newsletterUrl = $"https://newsletters.cdemi.io/archives/{newsletterFilename}", hashTags = (await newsletterCuratorContext.NewsitemsByCategory().Select(n => n.Key.Name).ToListAsync()) });
        }

        public IActionResult Share(string newsletterUrl, string[] hashTags)
        {
            return View(new ShareURLModel { URL = HttpUtility.UrlEncode(newsletterUrl), HashTags = hashTags });
        }

        private async Task<HttpResponseMessage> addToGitHubArchive(string source, string newsletterFilename)
        {
            var client = clientFactory.CreateClient("github");

            var request = new HttpRequestMessage(HttpMethod.Put, $"/repos/{configuration.GetValue<string>("GitHub:Username")}/{configuration.GetValue<string>("GitHub:ArchiveRepo")}/contents/archives/{newsletterFilename}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    message = $"Added newsletter for {DateTimeOffset.UtcNow.ToString("D", CultureInfo.CreateSpecificCulture("en-US"))}",
                    content = Convert.ToBase64String(Encoding.UTF8.GetBytes(source)),
                }), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request);
            return response;
        }

        public async Task<IActionResult> Preview()
        {
            var categoryNewsItemsViewModels = await newsletterCuratorContext.NewsitemsByCategory().Select(c => new CategoryNewsItemsViewModel { Category = c.Key, Newsitems = c.ToList() }).ToListAsync();

            return View(categoryNewsItemsViewModels);
        }
    }
}
