// models/user.model.ts
export interface UserModel {
    userId: number;
    loginId: number;
    domainId: number;
    userName: string;
    email: string;
    sysAdmin: boolean;
    environment: number;
  }