import { observer } from 'mobx-react-lite';
import { authStore } from '../stores/authStore';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import './Auth.css';
import { NotificationService } from '../services/notificationService';
import { registerValidationSchema, loginValidationSchema } from '../schemas/validationSchemas';
import { FormValues } from '../interfaces/FormValues';

const AuthComponent : React.FC = observer(() => {
  const handleRegister = async (
    { email, password }: FormValues, 
    { setSubmitting }: { setSubmitting: (isSubmitting: boolean) => void }
  ) => {
    await authStore.register(email, password);
    setSubmitting(false);
  };

  const handleLogin = async (
    { email, password }: FormValues,
    { setSubmitting }: { setSubmitting: (isSubmitting: boolean) => void }
  ) => {
    await authStore.login(email, password);
    setSubmitting(false);
  };

  const handleGenerateToken = async () => {
    await authStore.refreshToken();
  };

  const handleCopyToken = async () => {
    if (authStore.currentUser?.tokens.accessToken) {
      try {
        await navigator.clipboard.writeText(authStore.currentUser.tokens.accessToken);
        NotificationService.success("Token copied to clipboard!");
      } catch (error) {
        console.error('Failed to copy token', error);
      }
    }
  };

  return (
    <div className="container">
       <div className="section">
        <h2>API token</h2>
        <input type="text" disabled value={authStore.currentUser?.tokens.accessToken ?? "Login to get token"}></input>
        <button className="generate-token-button" onClick={handleGenerateToken} disabled={!authStore.isLoggedIn}>Generate</button>
        <button className="copy-token-button" onClick={handleCopyToken} disabled={!authStore.isLoggedIn}>Copy Token</button>
        </div>
      {!authStore.isLoggedIn && 
        <Formik
          initialValues={{ email: '', password: '', confirmPassword: '' }}
          validationSchema={registerValidationSchema}
          onSubmit={handleRegister}
        >
          {({ isSubmitting }) => (
            <Form className="section">
              <h2>Register</h2>
              <Field type="email" name="email" placeholder="Email" />
              <ErrorMessage name="email" component="div" className="error" />
              
              <Field type="password" name="password" placeholder="Password" />
              <ErrorMessage name="password" component="div" className="error" />

              <Field type="password" name="confirmPassword" placeholder="Confirm Password" />
              <ErrorMessage name="confirmPassword" component="div" className="error" />

              <button type="submit" disabled={isSubmitting}>Register</button>
            </Form>
          )}
        </Formik>
        }

        {!authStore.isLoggedIn && 
        <Formik
          initialValues={{ email: '', password: '' }}
          validationSchema={loginValidationSchema}
          onSubmit={handleLogin}
        >
          {({ isSubmitting }) => (
            <Form className="section">
              <h2>Log In</h2>
              <Field type="email" name="email" placeholder="Email" />
              <ErrorMessage name="email" component="div" className="error" />
              
              <Field type="password" name="password" placeholder="Password" />
              <ErrorMessage name="password" component="div" className="error" />

              <button type="submit" disabled={isSubmitting}>Log In</button>
            </Form>
          )}
        </Formik>
      }
    </div>
  );
});

export default AuthComponent;
