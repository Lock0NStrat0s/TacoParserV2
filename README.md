# Taco Bell Location Scraper
This project is a C# console application that:
1. Scrapes Taco Bell's website for location addresses, converts these addresses into geographic coordinates using an API and stores the resulting data in a CSV file
2. Parses through a CSV file, utilizes geolocation and logging 
3. Implements unit testing with XUnit

## Introduction
The Taco Bell Location Scraper project aims to automate the process of collecting Taco Bell location addresses from their official website, converting these addresses to geographic coordinates (latitude and longitude), and saving the information in a CSV file.
Once stored, the data is parsed through to extract the name and location to be used for geolocation and determining the distance between all the Taco bells.
This project demonstrates web scraping, API interaction, and data storage techniques in C#.

## Features
1. Web Scraping and API:
  - Scrapes location addresses from Taco Bell's website
  - Converts addresses to geographic coordinates using the OpenCage Geocoder API
  - Stores the address and coordinates in a CSV file

2. GeoLocation:
  - Finds the two Taco Bells that are the farthest apart from one another and shows the distance between them

3. Unit Testing:
  -  Checks if address, latitude and longitude match their respective values

## Install dependencies
The folowing packages are installed from the NuGet Package Manager:
  - HtmlAgilityPack
  - CsvHelper
  - Newtonsoft.Json
  - GeoCoordinate.NetCore
  - XUnit
  - DotNetEnv

## Usage
#### Configure API Key:
- You need an API key from OpenCage Geocoder
- Sign up on their [website](https://opencagedata.com/) to get your API key for testing purposes

#### Set the API key:
- You can store this in an .ENV file using this format -- *API_KEY=YOUR_KEY* -- in the main project directory

#### URL for testing:
- 4 scrapers have been built in this program:
  + Single Canadian city:
    + URL adjustable: Select any Canadian location with the format -- *`https://locations.tacobell.ca/en/[PROVINCE]/[CITY]`* --
    + Example: https://locations.tacobell.ca/en/ab/edmonton
  + Single US city:
    + URL adjustable: Select any US location with the format -- *`https://locations.tacobell.com/[STATE]/[CITY].html`* --
    + Example: https://locations.tacobell.com/al/huntsville.html
  + All Canadian cities:
    + URL fixed: https://www.tacobell.ca/en/store-locator.html
  + All US State cities:
    + URL adjustable: Pick from these [Taco Bell State Locations](https://locations.tacobell.com/) and use the URL of the State
