using SemestralniPraceDB2.Models.Entities;

namespace SemestralniPraceDB2.ViewModels;

public record class ViewChanged(string ViewName);
public record class UserLogin(Uzivatel prihlasenyUzivatel);
public record class UserLogout();
