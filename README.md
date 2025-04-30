# Introduction

Attempt to implement the GraphQL with [GraphQL.NET](https://graphql-dotnet.github.io/).

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

### Mutation

Here you can update Course's Description and add a new teacher.