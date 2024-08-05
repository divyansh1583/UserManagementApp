import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { AuthService } from "../services/auth.service";
import { TokenService } from "../services/token.service";

@Component({
    selector: 'app-activate-account',
    template: `
      <div class="container">
        <h2>Account Activation</h2>
        <p>{{message}}</p>
        <button *ngIf="activationSuccessful" (click)="navigateToLogin()" class="btn btn-primary">Go to Login</button>
      </div>
    `
})
export class ActivateAccountComponent implements OnInit {
    message: string = 'Activating your account...';
    activationSuccessful: boolean = false;

    constructor(
        private route: ActivatedRoute,
        private authService: AuthService,
        private router: Router,
        private toastr: ToastrService,
        private tokenService: TokenService,
    ) { }

    ngOnInit() {
        debugger
        this.route.queryParams.subscribe(params => {
            const token = params['code'];
            if (token) {
                var userId = this.tokenService.getUserIdToActivate(token);
                this.activateAccount(userId);
            } else {
                this.message = 'Invalid activation link';
                this.toastr.error(this.message);
            }
        });
    }

    activateAccount(userId: number) {
        this.authService.activateAccount(userId).subscribe({
            next: (res) => {
                if (res.statusCode === 200) {
                    this.message = res.message;
                    this.activationSuccessful = true;
                    this.toastr.success(res.message);
                } else {
                    this.message = res.message;
                    this.toastr.error(res.message);
                }
            },
            error: (err) => {
                this.message = 'An error occurred during activation';
                this.toastr.error(err.message);
            }
        });
    }

    navigateToLogin() {
        this.router.navigate(['/login']);
    }
}