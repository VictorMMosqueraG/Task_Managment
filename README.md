# Task Management

This project is a task management system developed by **VM Software**, where users can manage their tasks, assign priorities, and track progress.

## Table of Contents
- [Branch Naming Convention](#branch-naming-convention)
- [Environment Variables](#environment-variables)
- [Setup and Dependencies](#setup-and-dependencies)
- [Commit Message Structure](#commit-message-guidelines)
---

## Branch Naming Convention

In our project, we adhere to well-defined branch naming conventions to ensure a consistent and organized workflow. Our branch names are structured to provide clear information about their purpose and context.

### Naming Structure:
`[type]-[hu-name]-[plane]`

- **Type**: Represents the type of branch.
  - **FEATURE**: For developing new features.
  - **FIX**: For bug fixes or issue resolution.
  - **DOCS**: For changes or updates to the project documentation.
  - **REFACTOR**: For refactoring code without changing its functionality.
  - **TEST**: For creating or updating unit/integration tests.
  - **CI**: For changes related to CI/CD pipelines.
  - **HOTFIX**: For urgent fixes applied directly to production.
  - **CHORE**: For non-functional tasks like dependency updates or code cleanup.
  - **RELEASE**: For preparing code for a new version release.
  - **SP**: For research or exploration of a potential solution before implementation.
  - **BUGREPORT**: For creating or validating bug reports.
  - **MERGE**: For merging changes from different branches or resolving conflicts.
  - **STYLES**: For visual or design-related updates.
  - **CONFIGURATION**: For configuration or setup tasks.

- **hu-name**: Stands for the hypothetical user story name, which provides context for the branch.
- **plane**: Indicates the plane number associated with the task or issue.

### Example:
- **Feature Branch Example**:  
  Branch Name: `FEATURE-TASK-1`  
  Purpose: Implementation of a feature tied to task `TASK-1`.

By adhering to these branch naming conventions, we enhance clarity and traceability within our development process.

---

## Environment Variables

To configure environment variables for your project:

1. **Copy Template**:  
   Duplicate the `.env.template` file and rename it to `.env`.

2. **Edit Values**:  
   Open the newly created `.env` file with a text editor and replace the placeholder values with the appropriate configuration for your environment (such as database credentials, API keys, etc.).

By following these steps, you'll configure the necessary environment variables for your project without exposing sensitive information in your version control system.

---

## Setup and Dependencies

To set up and run this project, follow these steps:

1. **Clone the Repository**:  
   Clone this project repository to your local machine using Git. You can do this by running the following command:

   ```bash
   git clone https://github.com/VictorMMosqueraG/Task_Managment.git
    ```

2. **Install Dependencies**:
Ensure that you have all the necessary dependencies installed to work with .NET and ASP.NET Core. This includes:

    .NET SDK and Runtime
    Docker (for running containers such as PostgreSQL)

3. **Start the Database**:
Run the following command to start the PostgreSQL container:

    ```bash
    docker-compose up -d
    ```

4. Start the project:
After setting up the database, navigate to the project directory and run the following command to start the ASP.NET Core application:

    ```bash
    dotnet run
    ```
## Commit Message Guidelines

This project follows strict commit message guidelines to ensure that the commit history is clear, consistent, and easy to understand. These guidelines are enforced using **Husky** and **Commitlint**.

### Commit Message Structure

Each commit message should follow this structure:


- **Type**: Represents the type of change made in the commit. The type should be in uppercase, followed by a colon and a space.
  - **FEATURE**: For developing new features.
  - **FIX**: For bug fixes or issue resolution.
  - **DOCS**: For changes or updates to the project documentation.
  - **REFACTOR**: For refactoring code without changing its functionality.
  - **TEST**: For creating or updating unit/integration tests.
  - **CI**: For changes related to CI/CD pipelines.
  - **HOTFIX**: For urgent fixes applied directly to production.
  - **CHORE**: For non-functional tasks like dependency updates or code cleanup.
  - **RELEASE**: For preparing code for a new version release.
  - **SP**: For research or exploration of a potential solution before implementation.
  - **BUGREPORT**: For creating or validating bug reports.
  - **MERGE**: For merging changes from different branches or resolving conflicts.
  - **STYLES**: For visual or design-related updates.
  - **CONFIGURATION**: For configuration or setup tasks.

- **Subject**: A concise and clear description of the changes made in the commit. The first letter of the subject should be capitalized, and the rest of the message should be in lowercase (unless it's a proper noun or acronym).

### Examples of Correct Commit Messages

- `(FEATURE): Add user authentication`
- `(FIX): Resolve issue with database connection`
- `(DOCS): Update API documentation`
- `(REFACTOR): Simplify data processing logic`
- `(HOTFIX): Fix crash on login screen`

### Why Use These Commit Message Guidelines?

- **Clarity**: By following a consistent commit message format, it becomes easier to understand the context of each change.
- **Automated Versioning**: Tools like **semantic-release** can generate release notes and manage versioning based on commit types.
- **Better Collaboration**: With clear and standardized commit messages, team members can easily track changes and understand the history of the project.

---

By adhering to these commit message guidelines, we ensure a well-maintained and organized project history that improves both collaboration and automation.
