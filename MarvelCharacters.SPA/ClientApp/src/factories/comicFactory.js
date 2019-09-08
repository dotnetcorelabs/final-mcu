import t from 'prop-types';
import { ThumbnailFactory, thumbnailPropTypesSchema } from './thumbnailFactory';

function getShortDescription(description) {
    return description.substring(0, 36);
}

export const ComicFactory = ({
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

export const comicsPropTypesSchema = t.shape({
    id: t.string.isRequired,
    name: t.string.isRequired,
    description: t.string,
    resourceURI: t.string,
    thumbnail: thumbnailPropTypesSchema.isRequired
});