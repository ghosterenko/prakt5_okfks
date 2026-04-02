using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using Xunit;

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
    public void LoginWithValid()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("test@test.ru");
        driver.FindElement(By.Id("authPassword")).SendKeys("123456");
        driver.FindElement(By.Id("authSubmit")).Click();

        Assert.Contains("dashboard", driver.Url);
    }

    [Fact]
    public void LoginWithInvalidPassword()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("test@test.ru");
        driver.FindElement(By.Id("authPassword")).SendKeys("wrong");
        driver.FindElement(By.Id("authSubmit")).Click();

        var errorMessage = driver.FindElement(By.ClassName("error"));
        Assert.True(errorMessage.Displayed);
        Assert.Contains("https://test.webmx.ru/", driver.Url);
    }

    [Fact]
    public void LogoutFromSystem()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("test@test.ru");
        driver.FindElement(By.Id("authPassword")).SendKeys("123456");
        driver.FindElement(By.Id("authSubmit")).Click();

        driver.FindElement(By.Id("logout")).Click();

        Assert.Contains("https://test.webmx.ru/", driver.Url);
    }

    [Fact]
    public void CheckMainElementsAfterLogin()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("test@test.ru");
        driver.FindElement(By.Id("authPassword")).SendKeys("123456");
        driver.FindElement(By.Id("authSubmit")).Click();

        var menu = driver.FindElement(By.TagName("nav"));
        var content = driver.FindElement(By.TagName("main"));

        Assert.True(menu.Displayed);
        Assert.True(content.Displayed);
    }

    [Fact]
    public void NavigateBetweenSections()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("test@test.ru");
        driver.FindElement(By.Id("authPassword")).SendKeys("123456");
        driver.FindElement(By.Id("authSubmit")).Click();

        driver.FindElement(By.LinkText("Профиль")).Click();
        Assert.Contains("profile", driver.Url);

        driver.FindElement(By.LinkText("Заметки")).Click();
        Assert.Contains("notes", driver.Url);
    }

    [Fact]
    public void CreateNewNote()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("test@test.ru");
        driver.FindElement(By.Id("authPassword")).SendKeys("123456");
        driver.FindElement(By.Id("authSubmit")).Click();

        driver.FindElement(By.Id("createNote")).Click();
        driver.FindElement(By.Id("noteTitle")).SendKeys("Моя заметка");
        driver.FindElement(By.Id("noteContent")).SendKeys("Текст заметки");
        driver.FindElement(By.Id("saveNote")).Click();

        var successMessage = driver.FindElement(By.ClassName("success"));
        Assert.True(successMessage.Displayed);
    }

    [Fact]
    public void CreateNoteWithEmptyFields()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("test@test.ru");
        driver.FindElement(By.Id("authPassword")).SendKeys("123456");
        driver.FindElement(By.Id("authSubmit")).Click();

        driver.FindElement(By.Id("createNote")).Click();
        driver.FindElement(By.Id("saveNote")).Click();

        var errorMessage = driver.FindElement(By.ClassName("error"));
        Assert.True(errorMessage.Displayed);
    }

    [Fact]
    public void EditExistingNote()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("test@test.ru");
        driver.FindElement(By.Id("authPassword")).SendKeys("123456");
        driver.FindElement(By.Id("authSubmit")).Click();

        driver.FindElement(By.CssSelector(".note-item:first-child .edit")).Click();
        driver.FindElement(By.Id("noteContent")).Clear();
        driver.FindElement(By.Id("noteContent")).SendKeys("Новый текст");
        driver.FindElement(By.Id("saveNote")).Click();

        var successMessage = driver.FindElement(By.ClassName("success"));
        Assert.True(successMessage.Displayed);
    }

    [Fact]
    public void DeleteNote()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("test@test.ru");
        driver.FindElement(By.Id("authPassword")).SendKeys("123456");
        driver.FindElement(By.Id("authSubmit")).Click();

        driver.FindElement(By.CssSelector(".note-item:first-child .delete")).Click();
        driver.FindElement(By.Id("confirmDelete")).Click();

        var successMessage = driver.FindElement(By.ClassName("success"));
        Assert.True(successMessage.Displayed);
    }

    [Fact]
    public void SearchByExistingText()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("test@test.ru");
        driver.FindElement(By.Id("authPassword")).SendKeys("123456");
        driver.FindElement(By.Id("authSubmit")).Click();

        driver.FindElement(By.Id("search")).SendKeys("Моя заметка");
        driver.FindElement(By.Id("searchButton")).Click();

        var results = driver.FindElements(By.CssSelector(".note-item"));
        Assert.True(results.Count > 0);
    }

    [Fact]
    public void SearchByNonExistingText()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("test@test.ru");
        driver.FindElement(By.Id("authPassword")).SendKeys("123456");
        driver.FindElement(By.Id("authSubmit")).Click();

        driver.FindElement(By.Id("search")).SendKeys("zzzxyz123");
        driver.FindElement(By.Id("searchButton")).Click();

        var noResults = driver.FindElement(By.ClassName("no-results"));
        Assert.True(noResults.Displayed);
    }

    [Fact]
    public void CantSeeOtherUsersNotes()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("user1@test.ru");
        driver.FindElement(By.Id("authPassword")).SendKeys("pass1");
        driver.FindElement(By.Id("authSubmit")).Click();

        driver.FindElement(By.Id("search")).SendKeys("user2");
        driver.FindElement(By.Id("searchButton")).Click();

        var noResults = driver.FindElement(By.ClassName("no-results"));
        Assert.True(noResults.Displayed);
    }

    [Fact]
    public void NoEditButtonsAfterLogout()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("test@test.ru");
        driver.FindElement(By.Id("authPassword")).SendKeys("123456");
        driver.FindElement(By.Id("authSubmit")).Click();

        driver.FindElement(By.Id("logout")).Click();

        var editButtons = driver.FindElements(By.CssSelector(".edit"));
        Assert.Equal(0, editButtons.Count);
    }

    [Fact]
    public void SuccessMessageAfterSave()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("test@test.ru");
        driver.FindElement(By.Id("authPassword")).SendKeys("123456");
        driver.FindElement(By.Id("authSubmit")).Click();

        driver.FindElement(By.Id("createNote")).Click();
        driver.FindElement(By.Id("noteTitle")).SendKeys("Тест");
        driver.FindElement(By.Id("saveNote")).Click();

        var message = driver.FindElement(By.ClassName("success"));
        Assert.Contains("сохранено", message.Text.ToLower());
    }

    [Fact]
    public void ErrorMessageOnInvalidAction()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("test@test.ru");
        driver.FindElement(By.Id("authPassword")).SendKeys("123456");
        driver.FindElement(By.Id("authSubmit")).Click();

        driver.FindElement(By.CssSelector(".note-item:first-child .delete")).Click();
        driver.FindElement(By.Id("cancelDelete")).Click();

        var errorMessage = driver.FindElement(By.ClassName("error"));
        Assert.True(errorMessage.Displayed);
    }

    [Fact]
    public void CancelEditNoChanges()
    {
        driver.Url = "https://test.webmx.ru/";

        driver.FindElement(By.Id("authUsername")).SendKeys("test@test.ru");
        driver.FindElement(By.Id("authPassword")).SendKeys("123456");
        driver.FindElement(By.Id("authSubmit")).Click();

        string oldText = driver.FindElement(By.CssSelector(".note-item:first-child .content")).Text;

        driver.FindElement(By.CssSelector(".note-item:first-child .edit")).Click();
        driver.FindElement(By.Id("noteContent")).SendKeys("Временный текст");
        driver.FindElement(By.Id("cancelEdit")).Click();

        string newText = driver.FindElement(By.CssSelector(".note-item:first-child .content")).Text;
        Assert.Equal(oldText, newText);
    }
}
