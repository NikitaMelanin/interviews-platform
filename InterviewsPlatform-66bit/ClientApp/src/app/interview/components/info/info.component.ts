import {Component, OnDestroy, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute, Router} from "@angular/router";
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-start-interview',
  templateUrl: './info.component.html',
  styleUrls: ['./info.component.css'],
  providers: []

})


export class InfoComponent implements OnInit, OnDestroy {
  checkBoxForm!: FormGroup;
  submitted = false;

    description!: string;
    id!: string;

  constructor(private httpClient: HttpClient, private route: ActivatedRoute, private readonly router: Router, private formBuilder: FormBuilder) {
    this.id = this.route.snapshot.paramMap.get('passLink')!;
  }

  onSubmit() {
    this.router.navigate([this.id, 'process']);
    this.submitted = true;
    if (this,this.checkBoxForm.invalid) {
      return;
    }
  }
  get f() { return this.checkBoxForm.controls; }

  ngOnInit(): void {
    // this.httpClient.get<string>('https://localhost:44423/api/interviews/' + this.id+'/description').subscribe((x: any) => {
    //   this.description = x;
    // })
    this.checkBoxForm = this.formBuilder.group({
      chk1: ['', Validators.required],
      chk2: ['', Validators.required],
      chk3: ['', Validators.required],
    });
  }

  ngOnDestroy(): void {
  }


}
