namespace OnlineCourse.Primitives;

public class AccountLockedOutError() : Error(
    title: "Authentication.AccountLockedOut",
    detail: "This account has been locked out due to too many failed login attempts.");
