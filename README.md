# Introduction

Attempt to implement the GraphQL with [GraphQL.NET](https://graphql-dotnet.github.io/) and [HotChocolate](https://chillicream.com/docs/hotchocolate/v15).

To be honest, the documentation is too misleading.
So it was a pain to work with.

Aspire is used to simplify development.
`SqlServer` is used to keep the data.
`Entity Framework` to working with it.

## Initialize the Data

`Bogus` library is used to generate the data.
Just hit the `/create/` endpoint and it will generate it.

## Summary

We have a "University" where we have:
- Teachers
- Courses
- Students
- Course plan - which aggregate 3 above.

One Course Plan has one Course, one Teacher who teach this course, and several students (Though one student can have only one course).

### Query

Can get the list of everything.
And pass Name in each, except "Course Plan".
You can take each entity by Id, but use single name (eg. `teacher` vs. `teachers`).
</br>
Course plan is taken by `scheduler(s)`.
Linked parameters like `teacherId/courseId/etc` will be hidden from Client.

### Mutation

Implemented:
1. Update Course Description
2. Add new Teacher. Teacher as Input type without ID, as it will be added on Server.

## Data Loader

Allow to batch requests.
eg. we have a request of Courses Plans. Each of them have TeacherId that should resolve and return a Teacher.
</br>
Instead of making a request for each of Id, DataLoader firstly get all of those Id's, then pass them as List for resolving (via event?).

Students under the `scheduler` use `Data Loader`.