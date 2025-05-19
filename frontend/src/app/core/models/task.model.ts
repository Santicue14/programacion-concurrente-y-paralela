export class Task {
  id: number;
  title: string;
  description: string;
  completed: boolean;
  due_date: string;
  user_id: number;

  constructor(
    id: number,
    title: string,
    description: string,
    completed: boolean,
    due_date: string,
    user_id: number
  ) {
    this.id = id;
    this.title = title;
    this.description = description;
    this.completed = completed;
    this.due_date = due_date;
    this.user_id = user_id;
  }
}
