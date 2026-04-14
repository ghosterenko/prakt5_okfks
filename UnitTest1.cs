using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;

public class UnitTest1 : IDisposable
{
    IWebDriver driver = new EdgeDriver();

    public void Dispose()
    {
        driver.Dispose();
    }

    [Fact]
    public void OpenStartPageAndCheckTitle()
    {
        driver.Url = "https://test.webmx.ru/";
        var title = "Сервис заметок";
        Assert.Equal(title, driver.Title);
    }

    [Fact]
    public void CheckLoginFormUI()
    {
        driver.Url = "https://test.webmx.ru/";

        var login = driver.FindElement(By.Id("authUsername"));
        var password = driver.FindElement(By.Id("authPassword"));
        var button = driver.FindElement(By.Id("authSubmit"));

        Assert.True(login.Displayed);
        Assert.True(password.Displayed);
        Assert.True(button.Displayed);
    }

    [Fact]
    public void ErrorMessageOnEmptyLogin()
    {
        driver.Url = "https://test.webmx.ru/";

        var loginTb = driver.FindElement(By.Id("authUsername"));
        var passwordTb = driver.FindElement(By.Id("authPassword"));

        var requaredLogin = loginTb.GetAttribute("required");
        var requaredPassword = passwordTb.GetAttribute("required");

        Assert.NotNull(requaredLogin);
        Assert.NotNull(requaredPassword);
    }

    [Fact]
    public void LoginWithIncorrectCredentials()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("non");
        driver.FindElement(By.Id("authPassword")).SendKeys("111111");
        driver.FindElement(By.Id("authSubmit")).Click();

        var notes = driver.FindElements(By.Id("notesSection"));
        var hidden = notes[0].GetAttribute("class");
        Assert.Contains("hidden", hidden);
    }

    [Fact]
    public void LoginWithCorrectCredentials()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("non1");
        driver.FindElement(By.Id("authPassword")).SendKeys("111111");
        driver.FindElement(By.Id("authSubmit")).Click();

        Thread.Sleep(500);

        var notes = driver.FindElement(By.Id("notesSection"));
        Assert.True(notes.Displayed);
    }

    [Fact]
    public void LogoutFromAccount()
    {
        driver.Url = "https://test.webmx.ru/";
        driver.FindElement(By.Id("authUsername")).SendKeys("non1");
        driver.FindElement(By.Id("authPassword")).SendKeys("111111");
        driver.FindElement(By.Id("authSubmit")).Click();

        Thread.Sleep(500);

        var logoutButton = driver.FindElement(By.Id("logoutBtn"));
        logoutButton.Click();

        Thread.Sleep(500);

        var notes = driver.FindElements(By.Id("notesSection"));
        var hidden = notes[0].GetAttribute("class");
        Assert.Contains("hidden", hidden);
    }

    [Fact]
    public void CheckMainAreaAfterLogin()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("non1");
        driver.FindElement(By.Id("authPassword")).SendKeys("111111");
        driver.FindElement(By.Id("authSubmit")).Click();

        Thread.Sleep(500);

        var notes = driver.FindElement(By.Id("notesSection"));
        var add = driver.FindElement(By.Id("newNoteBtn"));
        var search = driver.FindElement(By.Id("searchInput"));
        var logout = driver.FindElement(By.Id("logoutBtn"));

        Thread.Sleep(500);

        Assert.True(notes.Displayed);
        Assert.True(add.Displayed);
        Assert.True(add.Enabled);
        Assert.True(search.Displayed);
        Assert.True(search.Enabled);
        Assert.True(logout.Displayed);
    }

    [Fact]
    public void CreateNewNote()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("non1");
        driver.FindElement(By.Id("authPassword")).SendKeys("111111");
        driver.FindElement(By.Id("authSubmit")).Click();

        Thread.Sleep(500);

        driver.FindElement(By.Id("newNoteBtn")).Click();
        driver.FindElement(By.Id("noteTitle")).SendKeys("test1");
        driver.FindElement(By.Id("noteContent")).SendKeys("test1");
        driver.FindElement(By.Id("saveBtn")).Click();

        Thread.Sleep(500);

        var note = driver.FindElement(By.XPath("/html/body/div/section[2]/div[2]/aside/ul/li/strong"));
        Assert.Contains("test", note.Text);
    }

    [Fact]
    public void CreateEmptyNote()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("non1");
        driver.FindElement(By.Id("authPassword")).SendKeys("111111");
        driver.FindElement(By.Id("authSubmit")).Click();

        Thread.Sleep(500);

        driver.FindElement(By.Id("newNoteBtn")).Click();

        Thread.Sleep(500);

        var noteTitle = driver.FindElement(By.Id("noteTitle"));
        var requaredTitle = noteTitle.GetAttribute("required");

        Assert.NotNull(requaredTitle);
    }

    [Fact]
    public void EditNote()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("non1");
        driver.FindElement(By.Id("authPassword")).SendKeys("111111");
        driver.FindElement(By.Id("authSubmit")).Click();

        Thread.Sleep(500);

        driver.FindElement(By.Id("newNoteBtn")).Click();
        driver.FindElement(By.Id("noteTitle")).SendKeys("tes");
        driver.FindElement(By.Id("noteContent")).SendKeys("test");
        driver.FindElement(By.Id("saveBtn")).Click();

        Thread.Sleep(500);


        driver.FindElement(By.Id("noteTitle")).Clear();
        driver.FindElement(By.Id("noteTitle")).SendKeys("test");
        driver.FindElement(By.Id("saveBtn")).Click();

        Thread.Sleep(500);

        var note = driver.FindElement(By.XPath("/html/body/div/section[2]/div[2]/aside/ul/li/strong"));
        Assert.Contains("test", note.Text);
    }

    [Fact]
    public void DeleteNote()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("non1");
        driver.FindElement(By.Id("authPassword")).SendKeys("111111");
        driver.FindElement(By.Id("authSubmit")).Click();

        Thread.Sleep(500);

        var notes = driver.FindElement(By.Id("notesList"));
        int countBefore = notes.FindElements(By.TagName("li")).Count;

        driver.FindElement(By.Id("newNoteBtn")).Click();
        driver.FindElement(By.Id("noteTitle")).SendKeys("test");
        driver.FindElement(By.Id("noteContent")).SendKeys("test");
        driver.FindElement(By.Id("saveBtn")).Click();
          
        Thread.Sleep(500);

        driver.FindElement(By.Id("deleteBtn")).Click();

        IAlert alert = driver.SwitchTo().Alert();
        alert.Accept();

        Thread.Sleep(500);

        Assert.Equal(countBefore, notes.FindElements(By.TagName("li")).Count);

    }

    [Fact]
    public void SearchNoteByTitle()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("non1");
        driver.FindElement(By.Id("authPassword")).SendKeys("111111");
        driver.FindElement(By.Id("authSubmit")).Click();

        Thread.Sleep(500);

        driver.FindElement(By.Id("newNoteBtn")).Click();
        driver.FindElement(By.Id("noteTitle")).SendKeys("testS");
        driver.FindElement(By.Id("noteContent")).SendKeys("testS");
        driver.FindElement(By.Id("saveBtn")).Click();

        Thread.Sleep(500);


        driver.FindElement(By.Id("searchInput")).SendKeys("testS");

        var note = driver.FindElement(By.XPath("/html/body/div/section[2]/div[2]/aside/ul/li/strong"));
        Assert.Contains("testS", note.Text);
    }

    [Fact]
    public void SearchWithEmptyResult()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("non1");
        driver.FindElement(By.Id("authPassword")).SendKeys("111111");
        driver.FindElement(By.Id("authSubmit")).Click();

        Thread.Sleep(500);

        driver.FindElement(By.Id("searchInput")).SendKeys("zxc");

        Thread.Sleep(500);

        var no = driver.FindElement(By.XPath("//*[@id=\"notesList\"]/li"));
        Assert.Contains("Нет заметок. Создайте первую заметку.", no.Text);
    }


    [Fact]
    public void SuccessMessageAfterCreateNote()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("non1");
        driver.FindElement(By.Id("authPassword")).SendKeys("111111");
        driver.FindElement(By.Id("authSubmit")).Click();

        Thread.Sleep(500);

        driver.FindElement(By.Id("newNoteBtn")).Click();
        driver.FindElement(By.Id("noteTitle")).SendKeys("Тест сообщения");
        driver.FindElement(By.Id("noteContent")).SendKeys("Содержимое");
        driver.FindElement(By.Id("saveBtn")).Click();

        Thread.Sleep(500);

        var message = driver.FindElement(By.Id("message"));
        var classe = message.GetAttribute("class");

        Assert.Contains("ok", classe);
        Assert.True(message.Displayed);
    }

    [Fact]
    public void SuccessMessageAfterUpdateNote()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("non1");
        driver.FindElement(By.Id("authPassword")).SendKeys("111111");
        driver.FindElement(By.Id("authSubmit")).Click();

        Thread.Sleep(500);

        driver.FindElement(By.Id("newNoteBtn")).Click();
        driver.FindElement(By.Id("noteTitle")).SendKeys("Для обновления");
        driver.FindElement(By.Id("noteContent")).SendKeys("Старый текст");
        driver.FindElement(By.Id("saveBtn")).Click();

        Thread.Sleep(500);

        driver.FindElement(By.Id("noteTitle")).Clear();
        driver.FindElement(By.Id("noteTitle")).SendKeys("Обновленная заметка");
        driver.FindElement(By.Id("saveBtn")).Click();

        Thread.Sleep(500);

        var message = driver.FindElement(By.Id("message"));
        Assert.Contains("ok", message.GetAttribute("class"));
    }

    [Fact]
    public void SuccessMessageAfterLogout()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("non1");
        driver.FindElement(By.Id("authPassword")).SendKeys("111111");
        driver.FindElement(By.Id("authSubmit")).Click();

        Thread.Sleep(500);

        driver.FindElement(By.Id("logoutBtn")).Click();

        Thread.Sleep(500);

        var message = driver.FindElement(By.Id("message"));
        Assert.Contains("ok", message.GetAttribute("class"));
    }

    [Fact]
    public void ErrorMessageOnLogin()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("non2");
        driver.FindElement(By.Id("authPassword")).SendKeys("111111");
        driver.FindElement(By.Id("authSubmit")).Click();

        Thread.Sleep(500);

        var message = driver.FindElement(By.Id("message"));
        var classe = message.GetAttribute("class");

        Assert.Contains("error", classe);
    }

}