import {Component, EventEmitter, Input, Output, OnChanges, SimpleChanges} from '@angular/core';
import {
    AbstractControl,
    FormArray,
    FormControl,
    FormGroup,
    ReactiveFormsModule,
    UntypedFormBuilder,
    Validators
} from "@angular/forms";
import {ErabliereApi} from "../core/erabliereapi.service";
import {CapteurImage} from "../model/capteurImage";
import {PutCapteurImage} from "../model/putCapteurImage";

@Component({
  selector: 'app-capteur-image-list',
  standalone: true,
    imports: [
        ReactiveFormsModule
    ],
  templateUrl: './capteur-image-list.component.html',
  styleUrl: './capteur-image-list.component.css'
})
export class CapteurImageListComponent implements OnChanges {
    @Input() idErabliere?: string;
    @Input() capteurs: CapteurImage[] = [];

    @Output() shouldRefreshCapteurs = new EventEmitter<void>();

    formArrayIdToKey: Map<string, number> = new Map<string, number>();
    form: FormGroup;
    displayEdits: { [id: string]: boolean } = {};
    editedCapteurs: { [id: string]: PutCapteurImage } = {};

    constructor(private readonly erabliereApi: ErabliereApi, private fb: UntypedFormBuilder) {
        this.form = this.fb.group({
            capteurs: new FormArray([])
        });
    }

    ngOnChanges(changes: SimpleChanges) {
        if(changes['capteurs']) {
            this.capteurs.forEach((capteur, key) => {
                this.formArrayIdToKey.set(capteur.id ?? '', key);
                const capteurFormGroup = this.fb.group({
                    nom: new FormControl(
                        capteur.nom,
                        {
                            validators: [
                                Validators.required,
                                Validators.maxLength(50)
                            ],
                            updateOn: 'blur'
                        }
                    ),
                    url: new FormControl(
                        capteur.url,
                        {
                            validators: [
                                Validators.required,
                                Validators.maxLength(2000),
                                Validators.pattern(/^rtsp:\/\/[-a-zA-Z0-9@:%._+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_+.~#?&/=]*)$/)
                            ],
                            updateOn: 'blur'
                        }
                    ),
                    port: new FormControl(
                        capteur.port,
                        {
                            validators: [
                                Validators.required,
                                Validators.maxLength(5)
                            ],
                            updateOn: 'blur'
                        }
                    ),
                    nomDUtilisateur: new FormControl(
                        capteur.identifiant,
                        {
                            validators: [
                                Validators.maxLength(200)
                            ],
                            updateOn: 'blur'
                        }
                    ),
                    motDePasse: new FormControl(
                        capteur.motDePasse,
                        {
                            validators: [
                                Validators.maxLength(200)
                            ],
                            updateOn: 'blur'
                        }
                    ),
                });
                this.getCapteurs().push(capteurFormGroup);
                this.displayEdits[capteur.id ?? ''] = false;
            });
        }
    }

    isDisplayEditForm(capteurId?: string): boolean {
        if (!capteurId) {
            return false;
        }

        return this.displayEdits[capteurId];
    }

    showModifierCapteur(capteur: CapteurImage) {
        if (capteur.id) {
            this.displayEdits[capteur.id] = true;
            const formCapteur = this.getCapteur(capteur.id);
            formCapteur.controls['nom'].setValue(capteur.nom);
            formCapteur.controls['url'].setValue(capteur.url);
            formCapteur.controls['port'].setValue(capteur.port);
            formCapteur.controls['nomDUtilisateur'].setValue(capteur.identifiant);
            formCapteur.controls['motDePasse'].setValue(capteur.motDePasse);
        }
    }

    hideModifierCapteur(capteur: CapteurImage) {
        if (capteur.id) {
            this.displayEdits[capteur.id] = false;
            this.editedCapteurs[capteur.id] = { ...capteur };
        }
    }

    modifierCapteur(capteur: CapteurImage) {
        if (!capteur.id) {
            console.error("capteur.id is undefined in modifierCapteur()");
        } else {
            const formCapteur = this.getCapteur(capteur.id);
            if (formCapteur.valid) {
                this.editedCapteurs[capteur.id] = {...capteur};
                this.editedCapteurs[capteur.id].nom = formCapteur.controls['nom'].value;
                this.editedCapteurs[capteur.id].url = formCapteur.controls['url'].value;
                this.editedCapteurs[capteur.id].port = formCapteur.controls['port'].value;
                this.editedCapteurs[capteur.id].identifiant = formCapteur.controls['nomDUtilisateur'].value;
                this.editedCapteurs[capteur.id].motDePasse = formCapteur.controls['motDePasse'].value;
                const putCapteur = this.editedCapteurs[capteur.id];
                this.erabliereApi.putCapteurImage(this.idErabliere, capteur.id, putCapteur).then(() => {
                    this.shouldRefreshCapteurs.emit();
                    if (capteur.id) {
                        this.displayEdits[capteur.id] = false;
                    }
                }).catch(() => {
                    alert('Erreur lors de la modification du capteur');
                });
            }
        }
    }

    async supprimerCapteur(capteur: CapteurImage) {
        if (confirm('Cette action est irréversible, vous perderez tout les données associé au capteur. Voulez-vous vraiment supprimer ce capteur?')) {
            try {
                await this.erabliereApi.deleteCapteurImage(this.idErabliere, capteur.id)
                this.shouldRefreshCapteurs.emit();
            }
            catch (e) {
                alert('Erreur lors de la suppression du capteur');
            }
        }
    }

    getCapteurs() {
        return this.form.get('capteurs') as FormArray;
    }

    getCapteur(capteurId: string) {
        const arrayKey = this.formArrayIdToKey.get(capteurId);
        return this.getCapteurs().at(arrayKey ?? 0) as FormGroup;
    }
    getOrdre(capteurId: string): AbstractControl<any, any> | null {
        return this.getCapteur(capteurId).controls['ordre'] ?? null;
    }
    getNom(capteurId: string): AbstractControl<any, any> | null {
        return this.getCapteur(capteurId).controls['nom'] ?? null;
    }
    getUrl(capteurId: string): AbstractControl<any, any> | null {
        return this.getCapteur(capteurId).controls['url'] ?? null;
    }
    getPort(capteurId: string) {
        return this.getCapteur(capteurId).controls['port'] ?? null;
    }
    getNomDUtilisateur(capteurId: string) {
        return this.getCapteur(capteurId).controls['nomDUtilisateur'] ?? null;
    }
    getMotDePasse(capteurId: string) {
        return this.getCapteur(capteurId).controls['motDePasse'] ?? null;
    }
    copyId(event: MouseEvent, capteurId?: string) {
        if (!capteurId) {
            return;
        }

        const button = event.target as HTMLButtonElement;
        button.innerText = "Copié!";
        navigator.clipboard.writeText(capteurId);
        setTimeout(() => {
            button.innerHTML = "&#x2398;"
        }, 750);
    }
}
