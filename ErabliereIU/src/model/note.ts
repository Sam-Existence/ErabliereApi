export class Note {
  id: any
  idErabliere: any
  title?: string
  text?: string
  fileExtension?: string
  file?: string
  created?: string
  noteDate?: string
  reminderDate?: string

  // calculated field
  decodedTextFile?: string
}
