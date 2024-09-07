export class BaseEntity {
    id!: string;
}

export class Container extends BaseEntity {
    name!: string;
    state!: string;
}

export enum ContainerCommands {
    Start,
    Stop,
    Delete
}