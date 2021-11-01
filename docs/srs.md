# Software Requirements

Rather than attempting to make a new issue tracker that multiple users will use in the future, 
the purpose of this issue tracker is to showcase technical skills while continuing to grow and 
learn new concepts/patterns/frameworks/etc. Because of this, the feature set may be limited to a 
bare minimum. However, even though the goal is learning, software engineering principles will be 
followed throughout such as clean code, testing, etc.

The biggest question is why an issue tracker? I feel that most everyone looking at this repo will 
be a developer of some kind and understand the concepts of an issue tracker. So rather than needing 
everyone to understand a concept and the code, I hope everyone already understands the concepts of 
issue tracking and can dive straight into the code.

Lastly, the issue tracker will start as an API only project. It may evolve to include a front end 
at a future time, but in my current role I focus more on backend material.

## User Personas

At this time, there will be one main user of the system - David the Developer. David the Developer 
can be anyone from a student starting their first coding class all the way up to a principal developer 
with years of experience, or a product owner driving the development of a new system. Regardless of the 
role, this person knows what software development is and the role of an issue tracker in software 
development.

## User Stories

1. As a developer, I want to create an issue with a name and description so that I can track its 
progress.
2. As a developer, I want to update an issue so that I can keep the issue up to date with the most recent information.
3. As a developer, I want to delete an issue so that I can remove irrelevant or duplicate issues.
4. As a developer, I want to have history on each issue so I can have context when required.

## Data Model

An issue should contain the following data elements.

1. ID - An internal database ID and an easy to read, autoincrementing ID.
2. Title - A descriptive short name for the issue.
3. Description - Relevant data pertaining to the issue.
4. Priority - A scale of 1 to 5 to be able to show/sort by priority.
5. Created - created timestamp
6. Updated - last modified timestamp
7. Deleted - deleted timestamp if not null

## Non-Functional Requirements

1. Support multiple users - multiple users can be updating an issue at them same time. The system 
must be able to handle all of the updates/deletes.

## Future Iterations

While I don't know when, if any, of these features will be added, other ideas that come to mind are:

* state - Add state to each issue such as new, resolved, in progress, etc.
* authentication - track who changes the issues.
* issue pagination - support a pagination model when there are multiple pages of issues.
* projects - split into multiple projects instead of 1 issue tracker.
* tagging - support tags such as `API`, `mobile`, etc. added to each issue.
* comments - support users adding comments to issues.
