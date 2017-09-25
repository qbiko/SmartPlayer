import javax.swing.*;
import java.awt.*;

public class ContainerPanel extends JPanel {
    public ContainerPanel(Dimension dimension) {
        super(new BorderLayout());
        setMaximumSize(dimension);
    }
}
