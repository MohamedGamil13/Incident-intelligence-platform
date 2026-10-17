export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
}

export interface ResetPasswordRequest {
  email: string;
  newPassword: string;
  confirmPassword: string;
}

export interface LoginResult {
  token: string;
  expires: string;
}

export interface AuthResult {
  isSuccess: boolean;
  message: string;
  token: string | null;
  expiration: string | null;
  errors: string[] | null;
}

export interface AuthSession {
  token: string;
  expires: string;
  email: string;
  roles: string[];
}
