## **Com_LTD_take2**

This project is part of the Cyber Security course during the 3rd year of the Computer Science degree.

**Main functions of the site:**
1. Creating a user (Customer/Provider)
2. Logging in
3. Forgot password functionality using secured emails
4. Viewing a page for dummy internet bundles for phones (Customer/Provider)
5. Adding a bundle (Provider only)
6. Resetting the password (Customer/Provider)

**All pages use various methods of protection:**
A. Multiple requirements for user creation:
1. Password must be at least 10 characters long
2. Must include special characters, such as upper and lower case letters, at least one number, and a special character (*, !, ., @, etc.)
3. Hashing the password using SHA-2 + Salt + HMAC

B. Using cookies for user type recognition.

C. Resilient against HTML injection attacks and XSS-type attacks.

# Please note!
In order to prevent use of my presonal Azure account and Mailjet program, 
Keys we removed for the program and there for related functions will not work properly. 
