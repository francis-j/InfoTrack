# InfoTrack - Development Project

## Task

The CEO from InfoTrack is very interested in SEO and how this can improve Sales. Every morning he searches the keyword “land registry searches” in Google and counts down to see where and how many times their company, www.infotrack.co.uk sits on the list. Seeing the CEO do this every day, a smart software developer at InfoTrack decides to write a small application for him that will automatically perform this operation and return the result to the screen. They design and code some software that receives a string of keywords, and a string URL. This is then processed to return a string of numbers for where the resulting URL is found in Google. For example “1, 10, 33” or “0”. The CEO is only interested if their URL appears in the first 100 results.

## API

This is a standard .NET 9 Minimal API project with a single endpoint `/api/search`.

In January, Google made a change to require JavaScript for Google Search. This change was made with the intent to make automated use more difficult. There are also systems in place to temporarily ban IP addresses when misuse has been detected. Using a VPN is recommended while running this application, or configure this application for "offline" use.

##### Configure

The `appsettings.json` configuration file has the following configuration:

```
{
    "Search": {
        "Google": {
            // When enabled, the API will attempt to scrape Google,
            // otherwise will use an embedded HTML file for "offline" use.
            "Enabled": true,
            "UserAgent": "",
            "Cookie": ""
        }
    }
}
```

`UserAgent` and `Cookie` are required when enabled.

-   User Agent: Use one from https://www.zenrows.com/blog/user-agent-web-scraping#best
-   Cookie: Retrieve your cookie by going to https://www.google.co.uk/search?num=100&q=land+registry+searches, opening Dev Tools (F12) and running `document.cookie` in the console.

Note: Even with valid values for the above, Google may detect automated use of their website and can return a different webpage that cannot be scraped.

##### Run

```
cd ./Scraper.Api/Api
dotnet run -c Release
```

The application should now be running on `https://localhost:7083`.

## UI

This is a Vue.js + VITE application with TailwindCSS for styling.

##### Run

```
cd ./scraper-ui
npm i
npm run build
npm run preview
```
