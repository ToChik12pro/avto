using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using Xunit;

namespace AvtoShow.Tests
{
    public class AvtoShowPageTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl;

        public AvtoShowPageTests()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AcceptInsecureCertificates = true;

            _driver = new ChromeDriver(chromeOptions);
            _baseUrl = "https://localhost:7274"; // ваша базовая URL-адрес страницы
        }

        [Fact]
        public void HomePage_ShouldLoadSuccessfully()
        {
            // Открытие главной страницы
            _driver.Navigate().GoToUrl(_baseUrl);

            // Проверка заголовка страницы
            var pageTitle = _driver.Title;
            Assert.Equal("Главная страница - AvtoShow", pageTitle);
        }

        [Fact]
        public void Categories_ShouldBeDisplayed()
        {
            // Открытие главной страницы
            _driver.Navigate().GoToUrl(_baseUrl);

            // Проверка отображения категорий автомобилей
            var categories = _driver.FindElements(By.CssSelector(".button-container a"));
            Assert.Equal(3, categories.Count);
        }

        [Fact]
        public void AvailableCars_ShouldBeDisplayed()
        {
            // Открытие главной страницы
            _driver.Navigate().GoToUrl(_baseUrl);

            // Проверка отображения доступных автомобилей
            var availableCars = _driver.FindElements(By.CssSelector(".swiper-slide"));
            Assert.Equal(5, availableCars.Count);
        }

        [Fact]
        public void CheckCategoryNames()
        {
            // Открытие главной страницы
            _driver.Navigate().GoToUrl(_baseUrl);

            // Проверка имен категорий автомобилей
            var categories = _driver.FindElements(By.CssSelector(".button-container span"));
            Assert.Collection(categories,
                item => Assert.Equal("Эконом", item.Text),
                item => Assert.Equal("Комфорт", item.Text),
                item => Assert.Equal("Элит", item.Text));
        }

        [Fact]
        public void CheckAvailableCarBrands()
        {
            // Открытие главной страницы
            _driver.Navigate().GoToUrl(_baseUrl);

            // Проверка брендов доступных автомобилей
            var carBrands = _driver.FindElements(By.CssSelector(".swiper-slide img"));
            Assert.Collection(carBrands,
                item => Assert.Contains("Logan", item.GetAttribute("alt")),
                item => Assert.Contains("Camry", item.GetAttribute("alt")),
                item => Assert.Contains("BMW", item.GetAttribute("alt")),
                item => Assert.Contains("Mercedes", item.GetAttribute("alt")),
                item => Assert.Contains("citroen", item.GetAttribute("alt")));
        }

        public void Dispose()
        {
            _driver.Quit();
        }
    }
}