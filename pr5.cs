using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace prakt5
{
    public class UnitTest1: IDisposable
    {
        IWebDriver driver = new ChromeDriver( );

        public void Dispose ()
        {
            driver.Dispose( );
        }

        [Fact]
        public void OpenStartPageAndCheckTitle ()
        {
            driver.Url = "https://test.webmx.ru/";
            var title = "Сервис заметок";
            Assert.Equal(title, driver.Title);
            driver.Close( );
        }
        [Fact]
        public void NavigateNoAutorizationUser()
        {
            driver.Url = "https://test.webmx.ru/";
            var buttonRegister = driver.FindElement(By.Id("registerTab"));
            Assert.True(buttonRegister.Displayed);
            driver.Close( );
        }
        [Fact]
        public void CheckStartUI()
        {
            driver.Url = "https://test.webmx.ru/";

            var loginTb = driver.FindElement(By.Id("authUsername"));
            var passwordTb = driver.FindElement(By.Id("authPassword"));
            var button = driver.FindElement(By.Id("authSubmit"));

            Assert.True(loginTb.Displayed);
            Assert.True(passwordTb.Displayed);
            Assert.True(button.Displayed);

            driver.Close( );
        }

        [Fact]
        public void SwitchModeOnPage()
        {
            driver.Url = "https://test.webmx.ru/";

            driver.FindElement(By.Id("registerTab")).Click();
            var btn = driver.FindElement(By.Id("authSubmit")).Text;
            Assert.Equal("Зарегистрироваться", btn);
            driver.FindElement(By.Id("loginTab")).Click( );
            btn = driver.FindElement(By.Id("authSubmit")).Text;
            Assert.Equal("Войти", btn);
            driver.Close( );
        }

        [Fact]
        public void LoginTryInvalidDataPassword()
        {
            driver.Url = "https://test.webmx.ru/";

            driver.FindElement(By.Id("authUsername")).SendKeys("test@test.ru");
            driver.FindElement(By.Id("authPassword")).SendKeys("fffff");
            driver.FindElement(By.Id("authSubmit")).Click( );

            var regbtn = driver.FindElement(By.Id("registerTab"));

            Assert.True(regbtn.Displayed);

            driver.Close( );
        }
        [Fact]
        public void LoginTryValidData()
        {
            driver.Url = "https://test.webmx.ru/";

            driver.FindElement(By.Id("authUsername")).SendKeys("test@test.ru");
            driver.FindElement(By.Id("authPassword")).SendKeys("ffffff");
            driver.FindElement(By.Id("authSubmit")).Click( );

            var message = driver.FindElement(By.XPath("/html/body/div/div/span")).Text;
            Assert.Equal("Неверный логин или пароль.", message);

            driver.Close( );
        }

        [Fact]
        public void LoginUser()
        {
            driver.Url = "https://test.webmx.ru/";

            driver.FindElement(By.Id("authUsername")).SendKeys("test1@test.ru");
            driver.FindElement(By.Id("authPassword")).SendKeys("ffffff");
            driver.FindElement(By.Id("authSubmit")).Click( );

            var testBtn = driver.FindElement(By.Id("newNoteBtn"));

            Assert.True(testBtn.Displayed);

            driver.Close();
        }
        [Fact]
        public void ExitUser()
        {
            driver.Url = "https://test.webmx.ru/";

            driver.FindElement(By.Id("authUsername")).SendKeys("test1@test.ru");
            driver.FindElement(By.Id("authPassword")).SendKeys("ffffff");
            driver.FindElement(By.Id("authSubmit")).Click( );

            driver.FindElement(By.Id("logoutBtn")).Click();

            driver.Close( );
        }

    }
}
