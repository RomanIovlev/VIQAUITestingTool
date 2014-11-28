package examples.site.pages;

import VIElements.BaseClasses.VIElement;
import VIElements.Interfaces.IButton;
import VIElements.Interfaces.ITextField;
import examples.data.User;
import org.openqa.selenium.support.FindBy;

/**
 * Created by roman.i on 07.11.2014.
 */
public class LoginForm extends VIElement {
    @FindBy(css = "#login-id")
    private ITextField login;

    @FindBy(css = "#psw-id")
    private ITextField password;

    @FindBy(css = "#loginBtn-id")
    private IButton loginButton;

    public void login(User user) throws Exception {
        login.newInput(user.login);
        password.newInput(user.password);
        loginButton.click();
    }

    public LoginForm() throws Exception {
    }
}
