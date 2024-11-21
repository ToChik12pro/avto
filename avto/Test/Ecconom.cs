
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace AvtoShow.Tests
{
    public class EconomPageTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl;

        public EconomPageTests()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AcceptInsecureCertificates = true;

            _driver = new ChromeDriver(chromeOptions);
            _baseUrl = "https://localhost:7274/Econom"; // ваш URL страницы "Эконом"
        }

        [Fact]
        public void PageTitle_ShouldBeCorrect()
        {
            // Открытие страницы "Эконом"
            _driver.Navigate().GoToUrl(_baseUrl);

            // Проверка заголовка страницы
            var pageTitle = _driver.Title;
            Assert.Equal("Эконом - AvtoShow", pageTitle);
        }

        [Fact]
        public void WelcomeMessage_ShouldBePresent()
        {
            // Открытие страницы "Эконом"
            _driver.Navigate().GoToUrl(_baseUrl);

            // Проверка наличия приветственного сообщения
            var welcomeMessage = _driver.FindElement(By.CssSelector(".containerModel h1")).Text;
            Assert.Equal("Добро пожаловать в сегмент эконом", welcomeMessage);
        }

        [Fact]
        public void CheckPopularCarModels()
        {
            // Открытие страницы "Эконом"
            _driver.Navigate().GoToUrl(_baseUrl);

            // Проверка популярных моделей автомобилей
            var carModels = _driver.FindElements(By.CssSelector(".col-md-4 .card-title"));
            Assert.Collection(carModels,
                item => Assert.Equal("Renault Logan", item.Text),
                item => Assert.Equal("Citroën C4", item.Text),
                item => Assert.Equal("Citroën JUMPY", item.Text));
        }

        [Fact]
        public void CheckCarTableHeaders()
        {
            // Открытие страницы "Эконом"
            _driver.Navigate().GoToUrl(_baseUrl);

            // Проверка заголовков таблицы с данными автомобилей
            var tableHeaders = _driver.FindElements(By.CssSelector(".table th"));
            Assert.Collection(tableHeaders,
                item => Assert.Equal("Модель", item.Text),
                item => Assert.Equal("Год выпуска", item.Text),
                item => Assert.Equal("VIN", item.Text),
                item => Assert.Equal("Цена", item.Text),
                item => Assert.Equal("Доступен", item.Text),
                item => Assert.Equal("Характеристики", item.Text),
                item => Assert.Equal("Категория", item.Text),
                item => Assert.Equal("Изображения", item.Text));
        }

        public void Dispose()
        {
            _driver.Quit();
        }
    }
}