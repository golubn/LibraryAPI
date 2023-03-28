![](./.github/logo.png)

EntityFramework Core + .NetCore + Authentication via bearer token + GRUD WebAPI + AutoMapper + Swagger + SQL Server

# Get started

# Start

- Clone repository.
- Start project by click on button.

  ![image](./github/button.png)

# Work

- Register. Open Auth. Run the POST query Register. Click "Try it out".

![image](./github/authPost.png)

- Enter your login and password and click "Execute".

![image](./github/enter.png)

- Execute login query.

![image](./github/login.png)

- Copy your token in response body.

![image](./github/token.png)

- CLick button Authorize and enter your token as a picture. Insert "bearer " insert the token.

![image](./github/auth.png)

![image](./github/entertoken.png)

- In order to get admin rights(update books, delete book, find by ID) go to database. Change role to 0 in Users table.
  If you have role : 1 you can only see all books and find book by isbn.

![image](./github/bd.png)

![image](./github/role.png)

- Work with books

![image](./github/book.png)
