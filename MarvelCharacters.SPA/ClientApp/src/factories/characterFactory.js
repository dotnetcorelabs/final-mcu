import t from 'prop-types';

function getShortDescription(description) {
    return description.substring(0, 36);
}

export const CharacterFactory = ({
    id = '',
    name = '',
    description = '',
    resourceURI = '',
    thumbnail = ThumbnailFactory({})
} = {}) => ({
    id: id,
    name: name,
    description: description,
    shortDescription: getShortDescription(description),
    resourceURI: resourceURI,
    thumbnail: ThumbnailFactory(thumbnail)
});

function GetThumbnail(path, extension) {
    return `${path}/portrait_medium.${extension}`;
}

export const ThumbnailFactory = ({
    path = '',
    extension = ''
} = {}) => ({
    path: path,
    extension: extension,
    fullPath: GetThumbnail(path, extension)
});

export const thumbnailPropTypesSchema = t.shape({
    path: t.string.isRequired,
    extension: t.string.isRequired
});

export const characterPropTypesSchema = t.shape({
    id: t.string.isRequired,
    name: t.string.isRequired,
    description: t.string,
    resourceURI: t.string,
    thumbnail: thumbnailPropTypesSchema.isRequired
});