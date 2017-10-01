import javax.imageio.ImageIO;
import javax.swing.*;
import javax.swing.border.Border;
import javax.swing.border.EmptyBorder;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.image.BufferedImage;
import java.io.File;
import java.util.*;
import java.util.List;

public class MainFrame extends JFrame {
    Player player;

    public MainFrame(String title) throws Exception {
        super(title);

        Container container = getContentPane();
        setLayout(new BoxLayout(container, BoxLayout.LINE_AXIS));


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
        clubNameHeaderPanel.setBorder(BorderFactory.createEmptyBorder(0,100,0,100));

        leftPanel.add(clubNameHeaderPanel, BorderLayout.NORTH);

        // 1.2 Grafika boiska
        JPanel fieldPanel = new JPanel(new BorderLayout());
        fieldPanel.setBorder(BorderFactory.createEmptyBorder(10, 0, 0, 0));

        BufferedImage fieldImage = ImageIO.read(new File(new File("").getAbsolutePath().concat("\\src\\images\\field.png")));
        JLabel fieldPicLabel = new JLabel(new ImageIcon(fieldImage));
        add(fieldPicLabel);
        fieldPanel.add(fieldPicLabel, BorderLayout.NORTH);

        leftPanel.add(fieldPanel, BorderLayout.CENTER);


        //  1.3 Wykres pulsu
        List<Double> scores = new ArrayList<>();
        Random random = new Random();
        int maxDataPoints = 40;
        int maxScore = 10;
        for (int i = 0; i < maxDataPoints; i++) {
            scores.add((double) random.nextDouble() * maxScore);
//            scores.add((double) i);
        }
        GraphPanel splotPanel = new GraphPanel(scores);
        splotPanel.setPreferredSize(new Dimension(900, 200));

        leftPanel.add(splotPanel, BorderLayout.SOUTH);

        //  2. Prawy panel
        //  2.1 Lista zawodników
        JPanel playersPanel = new JPanel();
        playersPanel.setLayout(new BoxLayout(playersPanel, BoxLayout.PAGE_AXIS));
        for(int i=1;i<19;i++){
            player = new Player(i, "Kuba", "Chodorowski", 21, 75, 183, "GK");
        }
        final JList playerJList = new JList(Player.playerList);
        playerJList.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
        playerJList.setSelectedIndex(Player.playerList.size()-1);
        playerJList.setVisibleRowCount(3);
        playerJList.setMaximumSize(new Dimension(500, 300));
        playersPanel.add(playerJList);

        // 2.2 Przyciski Dodaj, Edytuj, Usuń

        JPanel buttonsPanel = new JPanel(new BorderLayout());
        JButton addPlayer = new JButton("Dodaj");
        JButton editPlayer = new JButton("Edytuj");
        JButton removePlayer = new JButton("Usuń");

        buttonsPanel.add(editPlayer, BorderLayout.WEST);
        buttonsPanel.add(addPlayer, BorderLayout.CENTER);
        buttonsPanel.add(removePlayer, BorderLayout.EAST);

        buttonsPanel.setMaximumSize(new Dimension(500, 150));

        playersPanel.add(buttonsPanel);

        rightPanel.add(playersPanel, BorderLayout.NORTH);

        //  2.3 Szczegóły zawodnika
        JPanel detailsPlayerPanel = new JPanel(new BorderLayout());
        detailsPlayerPanel.setMaximumSize(new Dimension(500, 200));
        detailsPlayerPanel.setBorder(BorderFactory.createEmptyBorder(0,0,100,0));

        JPanel detailsLeftSide = new JPanel();
        detailsLeftSide.setBorder(BorderFactory.createEmptyBorder(20,10,0,0));
        detailsLeftSide.setLayout(new BoxLayout(detailsLeftSide, BoxLayout.PAGE_AXIS));
        JPanel detailsRightSide = new JPanel();
        detailsRightSide.setBorder(BorderFactory.createEmptyBorder(20,0,0,10));
        detailsRightSide.setLayout(new BoxLayout(detailsRightSide, BoxLayout.PAGE_AXIS));

        detailsPlayerPanel.add(detailsLeftSide, BorderLayout.WEST);
        detailsPlayerPanel.add(detailsRightSide, BorderLayout.EAST);

        JLabel namePlayer = new JLabel(player.getNumber() + ". " + player.getFirstname() + " " + player.getLastname(), SwingConstants.CENTER);
        namePlayer.setFont(new Font(clubNameLabel.getName(), Font.PLAIN, 25));
        detailsPlayerPanel.add(namePlayer, BorderLayout.NORTH);
        JLabel age = new JLabel("Wiek: " + player.getAge());
        age.setFont(new Font(clubNameLabel.getName(), Font.PLAIN, 15));
        detailsLeftSide.add(age);
        JLabel weight = new JLabel("Waga: " + player.getWeight());
        weight.setFont(new Font(clubNameLabel.getName(), Font.PLAIN, 15));
        detailsRightSide.add(weight);
        JLabel position = new JLabel("Pozycja: " + player.getPosition());
        position.setFont(new Font(clubNameLabel.getName(), Font.PLAIN, 15));
        detailsLeftSide.add(position);
        JLabel height = new JLabel("Wzrost: " + player.getHeight());
        height.setFont(new Font(clubNameLabel.getName(), Font.PLAIN, 15));
        detailsRightSide.add(height);
        JLabel kilometres = new JLabel("Przebyte km: ");
        kilometres.setFont(new Font(clubNameLabel.getName(), Font.PLAIN, 15));
        detailsLeftSide.add(kilometres);
        JLabel burnedCalories = new JLabel("Spalone kalorie: ");
        burnedCalories.setFont(new Font(clubNameLabel.getName(), Font.PLAIN, 15));
        detailsRightSide.add(burnedCalories);

        JLabel heartRate = new JLabel("Puls: ", SwingConstants.CENTER);
        heartRate.setFont(new Font(clubNameLabel.getName(), Font.PLAIN, 20));
        detailsPlayerPanel.add(heartRate, BorderLayout.SOUTH);

        rightPanel.add(detailsPlayerPanel, BorderLayout.SOUTH);


        playerJList.addListSelectionListener(new ListSelectionListener(){
            @Override
            public void valueChanged(ListSelectionEvent e){
                player = (Player)playerJList.getSelectedValue();
                namePlayer.setText(player.getNumber() + ". " + player.getFirstname() + " " + player.getLastname());
                age.setText("Wiek: " + player.getAge());
                weight.setText("Waga: " + player.getWeight());
                position.setText("Pozycja: " + player.getPosition());
                height.setText("Wzrost: " + player.getHeight());
                kilometres.setText("Przebyte km: ");
                burnedCalories.setText("Spalone kalorie: ");
                heartRate.setText("Puls: ");

                invalidate();
                validate();
                repaint();
            }
        });

    }
}
