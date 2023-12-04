using SemestralniPraceDB2.Models.Entities;

namespace SemestralniPraceDB2.ViewModels;

public record class ViewChanged(string ViewName);
public record class UserLogin(Uzivatel prihlasenyUzivatel);
public record class UserLogout();
public record class UserEmulation(Uzivatel emulovanyUzivatel);
public record class UserStopEmulation();
