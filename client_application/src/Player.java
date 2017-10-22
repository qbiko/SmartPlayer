import javax.swing.*;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class Player {
    public static DefaultListModel<Player> playerList = new DefaultListModel<>();
    private int number;
    private String firstname;
    private String lastname;
    private int age;
    private double weight;
    private double height;
    private String position;

    public Player(int number, String firstname, String lastname, int age, double weight, double height, String position) {
        this.number = number;
        this.firstname = firstname;
        this.lastname = lastname;
        this.age = age;
        this.weight = weight;
        this.height = height;
        this.position = position;
        playerList.addElement(this);
    }

    public int getNumber() {
        return number;
    }

    public String getFirstname() {
        return firstname;
    }

    public String getLastname() {
        return lastname;
    }

    public int getAge() {
        return age;
    }

    public double getWeight() {
        return weight;
    }

    public double getHeight() {
        return height;
    }

    public String getPosition() {
        return position;
    }

    @Override
    public String toString() {
        return number + ". " + firstname + " " + lastname;
    }
}
