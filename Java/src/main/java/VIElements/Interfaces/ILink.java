package VIElements.Interfaces;

/**
 * Created by 12345 on 28.09.2014.
 */
public interface ILink extends IClickable, IText {
    String getReference() throws Exception;
}
