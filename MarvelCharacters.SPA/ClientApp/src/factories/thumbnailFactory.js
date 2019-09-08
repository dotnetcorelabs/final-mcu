import t from 'prop-types';

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