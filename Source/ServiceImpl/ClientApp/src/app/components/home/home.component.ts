import { Component } from '@angular/core';
import { CommonAPI } from '../../shared/api/common.api';
import { BaseViewModel } from '../../shared/viewmodels';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
    private success: boolean = undefined;

    constructor(private commonApi: CommonAPI) {
    }

    private async runTest(): Promise<void> {
        const response: BaseViewModel = await this.commonApi.e2eTest();
        this.success = response.success;
    }
}
