import javax.imageio.ImageIO;
import javax.swing.*;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.File;
import java.util.*;

public class MainFrame extends JFrame {
    public MainFrame(String title) throws Exception {
        super(title);

        Container container = getContentPane();
        setLayout(new BoxLayout(container, BoxLayout.LINE_AXIS));

        JTextArea textArea = new JTextArea(5,4);

        ContainerPanel leftPanel = new ContainerPanel(new Dimension(900, 900));

        ContainerPanel rightPanel = new ContainerPanel(new Dimension(500, 900));

        container.add(leftPanel);
        container.add(rightPanel);


        //1. Lewy panel

        // 1.1 Nazwa klubu i herb
        //Herb
        JPanel clubNameHeaderPanel = new JPanel(new BorderLayout());

        BufferedImage clubImage = ImageIO.read(new File(new File("").getAbsolutePath().concat("\\src\\images\\lesznosmall.png")));
        JLabel clubPicLabel = new JLabel(new ImageIcon(clubImage));
        add(clubPicLabel);
        clubNameHeaderPanel.add(clubPicLabel, BorderLayout.EAST);

        //Nazwa klubu
        JLabel clubNameLabel = new JLabel("KS Futsal Leszno");
        clubNameLabel.setFont(new Font(clubNameLabel.getName(), Font.PLAIN, 50));
        clubNameHeaderPanel.add(clubNameLabel, BorderLayout.WEST);

        leftPanel.add(clubNameHeaderPanel, BorderLayout.NORTH);

        // 1.2 Grafika boiska
        JPanel fieldPanel = new JPanel(new BorderLayout());

        BufferedImage fieldImage = ImageIO.read(new File(new File("").getAbsolutePath().concat("\\src\\images\\field.png")));
        JLabel fieldPicLabel = new JLabel(new ImageIcon(fieldImage));
        add(fieldPicLabel);
        fieldPanel.add(fieldPicLabel, BorderLayout.NORTH);

        leftPanel.add(fieldPanel, BorderLayout.CENTER);


        //  1.3 Wykres pulsu
        java.util.List<Double> scores = new ArrayList<>();
        Random random = new Random();
        int maxDataPoints = 40;
        int maxScore = 10;
        for (int i = 0; i < maxDataPoints; i++) {
            scores.add((double) random.nextDouble() * maxScore);
//            scores.add((double) i);
        }
        GraphPanel splotPanel = new GraphPanel(scores);
        splotPanel.setPreferredSize(new Dimension(900, 250));

        leftPanel.add(splotPanel, BorderLayout.SOUTH);

        //  2. Prawy panel
        //  2.1 Lista zawodników
        Player player = new Player(1, "Kuba", "Chodorowski", 21, 75, 183, "GK");
        final JList fruitList = new JList(Player.playerList);
        fruitList.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
        fruitList.setSelectedIndex(0);
        fruitList.setVisibleRowCount(3);
        fruitList.setMaximumSize(new Dimension(500, 200));
        rightPanel.add(fruitList);
        //  2.2 Szczegóły zawodnika


    }
}
