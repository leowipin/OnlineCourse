namespace OnlineCourse.Primitives;

public class AccountLockedOutError(Guid id) : Error(
    code: "AUTH_ACCOUNT_LOCKED_OUT",
    title: "Account Locked Out Due to Failed Login Attempts",
    detail: $"The user account with ID '{id}' has been locked out after exceeding the maximum allowed failed login attempts.")
{ }
