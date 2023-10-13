import { Component } from '@angular/core';
import { Login } from '../contracts/login';
import { AuthService } from '../services/auth-service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})

export class LoginComponent {
  login: Login = {
    email: '',
    senha: ''
  }
  tipoOlho:string = 'fa-eye-slash'
  tipoInputSenha: string = 'password';
  
  constructor(private authService: AuthService) { }
  
  mostrarSenha() {
    if(this.tipoOlho == 'fa-eye-slash'){
      this.tipoOlho = 'fa-eye'
      this.tipoInputSenha = 'text'

    } else {
      this.tipoOlho = 'fa-eye-slash'
      this.tipoInputSenha = 'password'
    }
  }

  onSubmit() {
    this.authService.login(this.login.email, this.login.senha)
      .subscribe({
        next: (response) => {
          // Lógica para lidar com a resposta do servidor após o login bem-sucedido
          Swal.fire('Login realizado com sucesso!')
        },
        error: (error) => {
          console.error('Erro:', error);
  
          if (error.status === 401) {
            // Lógica para lidar com erro 403 (Acesso Proibido)
            Swal.fire({
              icon: 'error',
              title: 'Erro ao validar o usuário',
              text: `${error.error}`
            })
          } else if (error.status === 404) {
            // Lógica para lidar com erro 404 (Recurso Não Encontrado)
            Swal.fire({
              icon: 'error',
              title: 'Oops...',
              text: 'Página não encontrada!'
            })
          } else {
            // Lógica para lidar com outros erros
            Swal.fire({
              icon: 'error',
              title: 'Oops...',
              text: 'Erro inesperado'
            })
          }
        }
      });
  }
}
