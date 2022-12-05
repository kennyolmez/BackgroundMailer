# BackgroundMailer
Mailing service using Coravel with in-memory queueing and background job retrying/queueing failed email sends.


This email service will generate a mock invoice (as a View.cshtml) upon submission and send it as a mail to specified email address.

If the mail fails to send, it will be persisted in the database and retried with regular intervals via the background service.

Just input an email address and click the button to get the invoice sent to the specified address.
