# Taco Bell Location Scraper
This project is a C# console application that:
1. Scrapes Taco Bell's website for location addresses, converts these addresses into geographic coordinates using an API and stores the resulting data in a CSV file
2. Parses through a CSV file, utilizes geolocation and logging 
3. Implements unit testing with XUnit

## Introduction
The Taco Bell Location Scraper project aims to automate the process of collecting Taco Bell location addresses from their official website, converting these addresses to geographic coordinates (latitude and longitude), and saving the information in a CSV file. This project demonstrates web scraping, API interaction, and data storage techniques in C#.

## Features
1. Web Scraping and API:
- Scrapes location addresses from Taco Bell's website
- Converts addresses to geographic coordinates using the OpenCage Geocoder API
- Stores the address and coordinates in a CSV file

2. GeoLocation:
- Finds the two Taco Bells that are the farthest apart from one another and shows the distance between them

3. Unit Testing:
-  Checks if address, latitude and longitude match their respective values

## Install dependencies:
The folowing packages are installed from the NuGet Package Manager:
- HtmlAgilityPack
- CsvHelper
- Newtonsoft.Json
- GeoCoordinate.NetCore
- XUnit
- DotNetEnv

## Usage
### Configure API Key:
You need an API key from OpenCage Geocoder. Sign up on their [website](https://opencagedata.com/) to get your API key for testing purposes.

### Set the API key:
You can store this in an .ENV file using this format --> *API_KEY=YOUR_KEY*
