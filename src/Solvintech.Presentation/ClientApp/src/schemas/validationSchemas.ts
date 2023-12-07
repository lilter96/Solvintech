import * as Yup from 'yup';
import { FormValues } from '../interfaces/FormValues';

export const registerValidationSchema: Yup.Schema<FormValues> = Yup.object().shape({
  email: Yup.string().email('Invalid email').required('Required'),
  password: Yup.string().min(6, 'Password must be at least 6 characters').required('Required'),
  confirmPassword: Yup.string().oneOf([Yup.ref('password')], 'Passwords must match').required('Required'),
});

export const loginValidationSchema: Yup.Schema<FormValues> = Yup.object().shape({
  email: Yup.string().email('Invalid email').required('Required'),
  password: Yup.string().required('Required'),
});