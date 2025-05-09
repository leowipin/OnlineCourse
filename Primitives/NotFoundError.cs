namespace OnlineCourse.Primitives;

public class NotFoundError (string resourceName, Guid id): Error(
    title:"Recurso no encontrado", 
    detail:$"El recurso '{resourceName}' con ID '{id}' no fue encontrado.")
{
    public string Resource { get; } = resourceName; 
    public Guid Id { get; } = id;
}